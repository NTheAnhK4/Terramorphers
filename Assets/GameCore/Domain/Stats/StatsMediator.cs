using System;
using System.Collections.Generic;

namespace GameCore.Domain.Stats
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers = new();
        public event EventHandler<Query> Queries;
          public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);
          public void AddModifier(StatModifier modifier, Action onRemoved = null)
          {
              modifier.OnRemoved = onRemoved;
              modifiers.AddLast(modifier);
              Queries += modifier.Handle;
              modifier.OnDispose += _ =>
              {
                  modifiers.Remove(modifier);
                  Queries -= modifier.Handle;
              };
          }

          public void RemoveModifier(StatModifier value)
          {
              var node = modifiers.First;
              while (node != null)
              {
                  var modifier = node.Value;
                  var nextNode = node.Next;
                  if (modifier == value) modifier.Dispose();
                  node = nextNode;
              }
          }
          public void HandleEvent<T>(T owner, EEffectTriggerType trigger)
          {
              var node = modifiers.First;
              while (node != null)
              {
                  var modifier = node.Value;
                  if(!node.Value.MarkedForRemoval) modifier.HandleEvent(owner,trigger);
                  node = node.Next;
              }
          }
          public void Update()
          {
              var node = modifiers.First;
              //Update all modifier
              while (node != null)
              {
                  var modifier = node.Value;
                  modifier.Update();
                  node = node.Next;
              }
              //Dispose any that are finished
              node = modifiers.First;
              while (node != null)
              {
                  var nextNode = node.Next;
                  if(node.Value.MarkedForRemoval) node.Value.Dispose();
                  node = nextNode;
              }
          }
    }
}