﻿using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface IHolidaysProvider
    {
        Task<ICollection<Holiday>> GetHolidays(int year);
    }
}