
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using CodiceApp.Gravatar;
    using GameCore.Utility;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Serialization;

    namespace UtilityAI.Considerations
    {
        [CreateAssetMenu(menuName = "UtilityAI/ConsiderationSystem", fileName = "ConsiderationSystem")]
        public class ConsiderationSystem : SerializedScriptableObject
        {
           
            [Serializable]
            public class OperationElement
            {
                [HorizontalGroup("Row", Width = 0.1f)]
                [HideLabel]
                public EOperationType OperationType;

                [HorizontalGroup("Row")]
                [ShowIf(nameof(UseCurve))]
                [InlineProperty]
                [HideLabel]
                [HideReferenceObjectPicker]
                public CurveEvaluator CurveEvaluator;


                [HorizontalGroup("Row")]
                [ShowIf(nameof(UseValue))]
                [HideLabel]
                public float Value;

                private bool UseCurve =>
                    CurveEvaluator != null &&
                    CurveEvaluator.Curve != null &&
                    CurveEvaluator.Curve.length > 0;

                private bool UseValue =>
                    OperationType == EOperationType.Value &&
                    !UseCurve;
            }
            [Serializable]
            public class FunctionData
            {
                [HorizontalGroup("row", Width = 0.05f)][HideLabel]
                public int Index;
                [HorizontalGroup("row", Width = .95f)]
                [VerticalGroup("row/data")]
                [TextArea] public string Text;
                [VerticalGroup("row/data")]
                [ReadOnly] public List<OperationElement> OperationElements = new();
            }

          

            [Serializable]
            public class CurveEvaluator
            {
                [HorizontalGroup("Inner", Width = .05f)] [HideLabel]
                public int Index;
                [HorizontalGroup("Inner", Width = .1f)] [HideLabel]
                public EContextType ContextType;

                [HorizontalGroup("Inner", Width = .1f)] [HideLabel]
                public EContextType ParamContextType;
                
                [HorizontalGroup("Inner", Width = 0.3f)]
                [HideLabel]
                [ValueDropdown(nameof(GetKeys))]
                [Searchable]
                public string Key;

                [HorizontalGroup("Inner", Width = 0.5f)]
                [HideLabel]
                public AnimationCurve Curve;

                public CurveEvaluator Clone()
                {
                    return new CurveEvaluator()
                    {
                        ContextType = this.ContextType,
                        Key = this.Key,
                        Curve = this.Curve,
                        ParamContextType = this.ParamContextType,
                        Index = this.Index
                    };
                    
                }

                public float Evaluate(ConsiderationContext considerationContext)
                {
                    Context context = considerationContext.Get(ContextType);
                    string key = Key;
                    if (ParamContextType != EContextType.None && considerationContext.HasParamKey(ParamContextType))
                    {
                       
                        key = string.Format(Key, considerationContext.GetParams(ParamContextType));
                       
                    }

                  
                    float rawInput = context.GetData<float>(key);
                    //if(Key == BlackBoardConstant.STAMINA_AVAILABILITY_RATIO || Key == BlackBoardConstant.ENTITY_NEARNESS_RATIO) Debug.Log($"[Test] {key} with {Mathf.Clamp01(Curve.Evaluate(rawInput))}");
                    return Mathf.Clamp01(Curve.Evaluate(rawInput));
                }

                public bool CanEvaluate(ConsiderationContext context)
                {
                    if (Curve == null || Curve.length == 0) return false;
                    if (!context.Has(ContextType))
                    {
                        Debug.Log($"[Test] {ContextType} is not existed");
                        return false;
                    }
                    return true;
                }
                private IEnumerable<string> GetKeys()
                {
                    return BlackBoardConstant.GetAllKeys();
                }
            }
            [SerializeField] private List<CurveEvaluator> curveEvaluators = new();
            [SerializeField] private List<FunctionData> operationFunctions = new();
           
            public enum EOperationType{Value, Muttiply, Add, Subtract, Divide, Max, Min,LeftParen, RightParen, Comma}
           
            public float Evaluate(int id, ConsiderationContext context)
            {
                if (id < 0 || id >= operationFunctions.Count) return 0;
                FunctionData functionData = operationFunctions[id];
                Stack<float> st = new();
                foreach (var token in functionData.OperationElements)
                {
                    if(token.OperationType == EOperationType.Value)
                    {
                        float valueOperation = token.Value;
                        if (token.CurveEvaluator != null && token.CurveEvaluator.CanEvaluate(context)) valueOperation = token.CurveEvaluator.Evaluate(context);
                        st.Push(valueOperation);
                        
                    }
                    else if (IsOperator(token.OperationType) || IsFunction(token.OperationType))
                    {
                        float b = st.Pop();
                        float a = st.Pop();
                        switch (token.OperationType)
                        {
                            case EOperationType.Add:
                                st.Push(a + b);
                                break;
                            case EOperationType.Subtract:
                                st.Push(a - b);
                                break;
                            case EOperationType.Muttiply:
                                st.Push(a * b);
                                break;
                            case EOperationType.Divide:
                                st.Push(a/b);
                                break;
                            case EOperationType.Max:
                                st.Push(Mathf.Max(a,b));
                                break;
                            case EOperationType.Min:
                                st.Push(Mathf.Min(a,b));
                                break;
                        }
                    }
                    
                }

                return Mathf.Clamp01(st.Pop());
            }
          
            bool IsOperator(EOperationType op)
            {
                return op == EOperationType.Add 
                       || op == EOperationType.Subtract 
                       || op == EOperationType.Muttiply 
                       || op == EOperationType.Divide;
            }

            bool IsFunction(EOperationType op) => op == EOperationType.Max || op == EOperationType.Min;
            bool IsValid(string expr)
            {
                return Regex.IsMatch(expr,
                    @"^(?:\s*(?:x\d+|\d+(\.\d+)?|\.\d+|\+|\-|\*|\/|\(|\)|max|min|,)\s*)+$");
            }

            private void OnValidate()
            {
                for (int i = 0; i < curveEvaluators.Count; ++i)
                {
                    var curve = curveEvaluators[i];
                    if (curve.Curve == null || curve.Curve.length == 0)
                    {
                        curve.Curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
                    }

                    curve.Index = i;
                }

                for (int i = 0; i < operationFunctions.Count; ++i)
                {
                    operationFunctions[i].Index = i;
                }
            }
    #if UNITY_EDITOR
            List<OperationElement> Operationalize(string operationFuntion)
            {
                List<OperationElement> result = new();
                int i = 0;
                while (i < operationFuntion.Length)
                {
                    if (char.IsWhiteSpace(operationFuntion[i])) { i++; continue; }
                    
                    //x[i]
                    if (operationFuntion[i] == 'x')
                    {
                        int j = i + 1;
                        while (j < operationFuntion.Length && char.IsDigit(operationFuntion[j])) ++j;

                       
                         OperationElement newOperationElement = new OperationElement()
                         {
                             OperationType = EOperationType.Value,
                             CurveEvaluator =  curveEvaluators[int.Parse(operationFuntion.Substring(i + 1, j - (i + 1)))].Clone()
                             
                         };
                        result.Add(newOperationElement);
                        i = j;
                       
                    }
                    //digital
                    else if (char.IsDigit(operationFuntion[i]))
                    {
                        int j = i;

                      
                        while (j < operationFuntion.Length && 
                               (char.IsDigit(operationFuntion[j]) || operationFuntion[j] == '.'))
                        {
                            j++;
                        }

                        string numStr = operationFuntion.Substring(i, j - i);
                        float value = float.Parse(numStr);

                        result.Add(new OperationElement()
                        {
                            OperationType = EOperationType.Value,
                            Value = value
                        });

                        i = j;
                    }
                    //function
                    else if (char.IsLetter(operationFuntion[i]))
                    {
                        if (operationFuntion.Substring(i).StartsWith("max"))
                        {
                            result.Add(new OperationElement()
                            {
                                OperationType = EOperationType.Max
                            });
                            i += 3;
                        }
                        else if (operationFuntion.Substring(i).StartsWith("min"))
                        {
                            result.Add(new OperationElement()
                            {
                                OperationType = EOperationType.Min
                            });
                            i += 3;
                        }
                    }
                    else if ("+-*/(),".Contains(operationFuntion[i]))
                    {
                        switch (operationFuntion[i])
                        {
                            case '+':
                                result.Add(new OperationElement(){OperationType = EOperationType.Add});
                                break;
                            case '-':
                                result.Add(new OperationElement(){OperationType = EOperationType.Subtract});
                                break;
                            case '*':
                                result.Add(new OperationElement(){OperationType = EOperationType.Muttiply});
                                break;
                            case '/':
                                result.Add(new OperationElement(){OperationType = EOperationType.Divide});
                                break;
                            case '(':
                                result.Add(new OperationElement(){OperationType = EOperationType.LeftParen});
                                break;
                            case ')':
                                result.Add(new OperationElement(){OperationType = EOperationType.RightParen});
                                break;
                            case ',':
                                result.Add(new OperationElement(){OperationType = EOperationType.Comma});
                                break;
                        }

                        ++i;
                    }
                    
                }

                return result;
            
            }
            int Priority(EOperationType op)
            {
                if (op == EOperationType.Add || op == EOperationType.Subtract) return 1;
                if (op == EOperationType.Muttiply || op == EOperationType.Divide) return 2;
                return 0;
            }
            private void ParseExpression(int id)
            {
                FunctionData functionData = operationFunctions[id];
                if (!IsValid(functionData.Text))
                {
                    Debug.LogError($"{functionData.Text} is not valid");
                    return;
                }
                functionData.OperationElements.Clear();
                Stack<OperationElement> stack = new();
               
                List<OperationElement> tokens = Operationalize(functionData.Text);
              
                foreach (var token in tokens)
                {
                    if (token.OperationType == EOperationType.Value) functionData.OperationElements.Add((token));
                    else if (token.OperationType == EOperationType.Min || token.OperationType == EOperationType.Max)  stack.Push(token);
                    else if (token.OperationType == EOperationType.Comma)
                    {
                        while (stack.Peek().OperationType != EOperationType.LeftParen)
                            functionData.OperationElements.Add(stack.Pop());
                    }
                    else if (IsOperator(token.OperationType))
                    {
                        while (stack.Count > 0
                               && IsOperator(stack.Peek().OperationType)
                               && Priority(stack.Peek().OperationType) >= Priority(token.OperationType))
                        {
                            functionData.OperationElements.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                    else if(token.OperationType == EOperationType.LeftParen) stack.Push(token);
                    else if (token.OperationType == EOperationType.RightParen)
                    {
                        while (stack.Peek().OperationType != EOperationType.LeftParen)
                            functionData.OperationElements.Add(stack.Pop());

                        stack.Pop(); // remove '('

                        if (stack.Count > 0 && IsFunction(stack.Peek().OperationType))
                            functionData.OperationElements.Add(stack.Pop());
                    }
                }
                while (stack.Count > 0)
                    functionData.OperationElements.Add(stack.Pop());
            }

            [Button]
            private void ParseAll()
            {
                for(int i = 0; i < operationFunctions.Count; ++i) ParseExpression(i);
            }

            [Button]
            private void Test(int id)
            {
                float value = Evaluate(id, new ConsiderationContext());
                Debug.Log(value);
            }
            
            #endif
        }
       

    }
