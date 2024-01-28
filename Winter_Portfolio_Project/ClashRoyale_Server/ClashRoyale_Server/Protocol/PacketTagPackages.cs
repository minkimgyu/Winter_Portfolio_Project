﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.ClashRoyale_Server.Protocol
{
    public enum Server_PacketTagPackages
    {
        S_LOAD_CARD_COLLECTION = 1,
        S_ACCEPT_REGISTER_ACCOUNT,
        S_REJECT_REGISTER_ACCOUNT,

        S_ACCEPT_LOGIN,
        S_REJECT_LOGIN,

        S_REQUEST_PLAY_GAME,

        S_ALERT_OVER_TIME,

        S_REQUEST_END_GAME,
    }

    public enum Client_PacketTagPackages
    {
        C_REQUEST_REGISTER_ACCOUNT = 1,
        C_REQUEST_LOGIN,
        C_REQUEST_ENTER_ROOM,

        C_ALERT_WIN,
        C_ALERT_LOOSE,

        C_REQUEST_END_GAME
    }
}