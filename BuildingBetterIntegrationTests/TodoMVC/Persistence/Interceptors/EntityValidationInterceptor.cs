using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using NHibernate;
using NHibernate.Event;

namespace TodoMVC.Persistence.Interceptors
{
    public class EntityValidationInterceptor : IPreInsertEventListener, IPreUpdateEventListener, IPreCollectionUpdateEventListener
    {
        private readonly AttributedValidatorFactory validatorFactory = new AttributedValidatorFactory();
        private static readonly ValidationResult emptyValidationResult = new ValidationResult();

        public bool OnPreInsert(PreInsertEvent @event)
        {
            Validate(@event.Entity, @event.Session.EntityMode);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            Validate(@event.Entity, @event.Session.EntityMode);
            return false;
        }

        public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
        {
            var owner = @event.AffectedOwnerOrNull;
            if (!ReferenceEquals(null, owner))
            {
                Validate(owner, @event.Session.EntityMode);
            }
        }

        protected void Validate(object entity, EntityMode mode)
        {
            if (entity == null || mode != EntityMode.Poco) return;

            var validator = validatorFactory.GetValidator(entity.GetType());
            var result = validator != null ? validator.Validate(entity) : emptyValidationResult;
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}