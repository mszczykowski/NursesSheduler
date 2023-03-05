﻿namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.DeleteDepartament
{
    public sealed class DeleteDepartamentResponse
    {
        public bool Success { get; set; }

        public DeleteDepartamentResponse(bool success)
        {
            Success = success;
        }
    }
}
