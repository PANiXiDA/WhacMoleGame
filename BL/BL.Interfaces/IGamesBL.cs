﻿using Common.ConvertParams;
using Common.SearchParams;
using Entities;

namespace BL.Interfaces
{
    public interface IGamesBL : ICrudBL<Game, GamesSearchParams, GamesConvertParams>
    {
    }
}
