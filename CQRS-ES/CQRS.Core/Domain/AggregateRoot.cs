using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id 
        { 
            get { return _id;}
        }

        public int Version { get; set;} = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges() 
        {
            return _changes;
        }

        /// <summary>
        /// MarkChangesAsCommitted() needs to be done after the uncommitted changes have, in fact, it altered the state of the aggregate
        /// </summary>
        public void MarkChangesAsCommitted() 
        {
            _changes.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="isNew">true if @event is a new event, false when @event has been taken from the event store</param>
        private void ApplyChange(BaseEvent @event, bool isNew) 
        {
            /* REFLECTION ???
                En programmation C#, la réflexion (reflection en anglais) est une fonctionnalité qui permet à un programme d'analyser sa propre structure 
                à l'exécution. Cela signifie qu'un programme peut examiner et manipuler ses types, méthodes, attributs, etc., 
                même s'ils n'étaient pas connus à la compilation.

                La réflexion permet d'effectuer des tâches telles que :

                - Examiner les types et les membres d'une classe à l'exécution.
                - Instancier dynamiquement des types.
                - Appeler des méthodes dynamiquement.
                - Accéder et manipuler des propriétés et des champs.

                using System;
                using System.Reflection;

                class Program
                {
                    static void Main()
                    {
                        // Obtention du type d'une classe
                        Type type = typeof(MyClass);

                        // Obtention des méthodes de la classe
                        MethodInfo[] methods = type.GetMethods();
                        Console.WriteLine("Méthodes de MyClass :");
                        foreach (MethodInfo method in methods)
                        {
                            Console.WriteLine(method.Name);
                        }

                        // Instanciation dynamique
                        object instance = Activator.CreateInstance(type);

                        // Appel de méthode dynamique
                        MethodInfo myMethod = type.GetMethod("MyMethod");
                        myMethod.Invoke(instance, null);
                    }
                }

                class MyClass
                {
                    public void MyMethod()
                    {
                        Console.WriteLine("Appel de MyMethod()");
                    }
                }

            
            */
            var method = this.GetType().GetMethod("Apply", new Type[]{@event.GetType() });

            if(method == null) 
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {@event.GetType().Name}!");
            }

            method.Invoke(this, new object[] {@event});

            if(isNew) 
            {
                _changes.Add(@event);
            }
        }

        /// <summary>
        /// RaiseEvent invokes the ApplyChange method when the event is new
        /// </summary>
        /// <param name="event"></param>
        protected void RaiseEvent(BaseEvent @event) 
        {
            ApplyChange(@event, true);
        }

        /// <summary>
        /// ReplayEvent invokes the ApplyChange method when the events are taken from the Event Store (events have been stored previously)
        /// </summary>
        /// <param name="events"></param>
        public void ReplayEvent(IEnumerable<BaseEvent> events) 
        {
            foreach(var @event in events) 
            {
                ApplyChange(@event, false);
            }
        }
    }
}