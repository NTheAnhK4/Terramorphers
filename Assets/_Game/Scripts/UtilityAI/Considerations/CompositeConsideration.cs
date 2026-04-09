using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace UtilityAI.Considerations
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/CompositeConsideration", fileName = "CompositeConsideration")]
    public class CompositeConsideration : Consideration
    {
        [Serializable]
        public class ConsiderationOperation
        {
            public EOperationType OperationType;
            public Consideration Consideration;
            public float value;
        }

        public enum EOperationType{Value, Muttiply, Add, Subtract, Divide, Max, Min,LeftParen, RightParen, Comma}

        [SerializeField, TextArea] private string operationFuntion;
        [SerializeField] private List<Consideration> considerations = new();
        [SerializeField, TableList] private List<ConsiderationOperation> considerationOperations = new();
        
        public override float Evaluate(Context context)
        {
            Stack<float> st = new();
            foreach (var token in considerationOperations)
            {
                if(token.OperationType == EOperationType.Value) st.Push(token.Consideration == null ? token.value : token.Consideration.Evaluate(context));
                else if (IsOperator(token.OperationType))
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
                    }
                }
                else if (IsFunction(token.OperationType))
                {
                    float b = st.Pop();
                    float a = st.Pop();
                    switch (token.OperationType)
                    {
                        case EOperationType.Min:
                            st.Push(Mathf.Min(a,b));
                            break;
                        case EOperationType.Max:
                            st.Push(Mathf.Max(a,b));
                            break;
                    }
                }
            }

            return st.Pop();
        }
        #if UNITY_EDITOR
        [Button]
        private void Test()
        {
            float value = Evaluate(new Context());
            Debug.Log(value);
        }
        [Button]
        private void ParseExpression()
        {
            if (!IsValid(operationFuntion))
            {
                Debug.LogError("operation function is not valid");
                return;
            }
            considerationOperations.Clear();
            Stack<ConsiderationOperation> stack = new();
            List<ConsiderationOperation> tokens = Operationalize();
          
            foreach (var token in tokens)
            {
                if (token.OperationType == EOperationType.Value) considerationOperations.Add((token));
                else if (token.OperationType == EOperationType.Min || token.OperationType == EOperationType.Max)  stack.Push(token);
                else if (token.OperationType == EOperationType.Comma)
                {
                    while (stack.Peek().OperationType != EOperationType.LeftParen)
                        considerationOperations.Add(stack.Pop());
                }
                else if (IsOperator(token.OperationType))
                {
                    while (stack.Count > 0
                           && IsOperator(stack.Peek().OperationType)
                           && Priority(stack.Peek().OperationType) >= Priority(token.OperationType))
                    {
                        considerationOperations.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
                else if(token.OperationType == EOperationType.LeftParen) stack.Push(token);
                else if (token.OperationType == EOperationType.RightParen)
                {
                    while (stack.Peek().OperationType != EOperationType.LeftParen)
                        considerationOperations.Add(stack.Pop());

                    stack.Pop(); // remove '('

                    if (stack.Count > 0 && IsFunction(stack.Peek().OperationType))
                        considerationOperations.Add(stack.Pop());
                }
            }
            while (stack.Count > 0)
                considerationOperations.Add(stack.Pop());
        }

        List<ConsiderationOperation> Operationalize()
        {
            List<ConsiderationOperation> result = new();
            int i = 0;
            while (i < operationFuntion.Length)
            {
                if (char.IsWhiteSpace(operationFuntion[i])) { i++; continue; }
                
                //x[i]
                if (operationFuntion[i] == 'x')
                {
                    int j = i + 1;
                    while (j < operationFuntion.Length && char.IsDigit(operationFuntion[j])) ++j;

                  
                     ConsiderationOperation newConsiderationOperation = new ConsiderationOperation()
                     {
                         OperationType = EOperationType.Value,
                         Consideration = considerations[int.Parse(operationFuntion.Substring(i + 1, j - (i + 1)))]
                     };
                    result.Add(newConsiderationOperation);
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

                    result.Add(new ConsiderationOperation()
                    {
                        OperationType = EOperationType.Value,
                        value = value
                    });

                    i = j;
                }
                //function
                else if (char.IsLetter(operationFuntion[i]))
                {
                    if (operationFuntion.Substring(i).StartsWith("max"))
                    {
                        result.Add(new ConsiderationOperation()
                        {
                            OperationType = EOperationType.Max
                        });
                        i += 3;
                    }
                    else if (operationFuntion.Substring(i).StartsWith("min"))
                    {
                        result.Add(new ConsiderationOperation()
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
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.Add});
                            break;
                        case '-':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.Subtract});
                            break;
                        case '*':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.Muttiply});
                            break;
                        case '/':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.Divide});
                            break;
                        case '(':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.LeftParen});
                            break;
                        case ')':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.RightParen});
                            break;
                        case ',':
                            result.Add(new ConsiderationOperation(){OperationType = EOperationType.Comma});
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
        #endif
    }
}