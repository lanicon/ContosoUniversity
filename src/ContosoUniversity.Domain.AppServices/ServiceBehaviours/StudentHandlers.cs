﻿namespace ContosoUniversity.Domain.AppServices.ServiceBehaviours
{
    using ContosoUniversity.Domain.Core.Behaviours.Students;
    using Core.Factories;
    using Core.Repository.Containers;
    using Core.Repository.Entities;
    using NRepository.Core;

    public static class StudentHandlers
    {
        // Create student
        public static StudentCreate.Response Handle(IRepository repository, StudentCreate.Request request)
        {
            var container = new EntityStateWrapperContainer();
            container.AddEntity(StudentFactory.Create(request.CommandModel));
            var validationDetails = repository.Save(container);

            var studentId = default(int?);
            if (!validationDetails.HasValidationIssues)
                studentId = container.FindEntity<Student>().ID;

            return new StudentCreate.Response(validationDetails, studentId);
        }

        // Modify student
        public static StudentModify.Response Handle(IRepository repository, StudentModify.Request request)
        {
            var commandModel = request.CommandModel;
            var container = StudentFactory.CreatePartial(commandModel.ID).Modify(commandModel);
            var validationDetails = repository.Save(container);

            return new StudentModify.Response(validationDetails);
        }

        // Delete studen
        public static StudentDelete.Response Handle(IRepository repository, StudentDelete.Request request)
        {
            var container = StudentFactory
                .CreatePartial(request.CommandModel.StudentId)
                .Delete();

            var validationDetails = repository.Save(container);

            return new StudentDelete.Response(validationDetails);
        }
    }
}