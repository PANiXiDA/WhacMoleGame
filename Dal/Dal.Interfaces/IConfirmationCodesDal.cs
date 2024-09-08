﻿using Common.SearchParams;
using Dal.DbModels;
using Dal.DbModels.Models;
using Dal.Interfaces.Core;

namespace Dal.Interfaces
{
    public interface IConfirmationCodesDal : IBaseDal<DefaultDbContext, ConfirmationCode, Entities.ConfirmationCode, int, ConfirmationCodesSearchParams, object>
    {
    }
}
