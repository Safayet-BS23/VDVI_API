﻿using System;
using System.Collections.Generic;
using System.Text;
using VDVI.DB.Models.ApmaModels;

namespace VDVI.DB.IRepository
{
    public  interface IManagementRoomSummaryRepository
    {
        void InsertRoomSummary(List<RoomSummary> roomSummary);
        void InsertLedgerBalance(List<LedgerBalance> ledgerBalance);
    }
}
