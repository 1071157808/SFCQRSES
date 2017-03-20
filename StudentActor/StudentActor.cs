﻿using System.Threading.Tasks;
using Common.DDD;
using Common.ServiceFabric.Extensions.Actors.Runtime;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using StudentActor.Events;
using StudentActor.Events.Implementation;
using StudentActor.Interfaces;
using Student = StudentActor.Domain.Student;

namespace StudentActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class StudentActor : EventStoredActorBase<Student, EventStream>, IStudentActor, IHandleDomainEvent<StudentRegisteredEvent>
    {
        public StudentActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task RegisterAsync(RegisterCommand command)
        {
            return Task.Run(() => DomainState.Register(this.GetActorId().GetGuidId(), command.Name));
        }

        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            await GetAndSetDomainAsync();
        }

        public async Task Handle(StudentRegisteredEvent domainEvent)
        {
            await StoreDomainEventAsync(domainEvent);
        }
    }
}