﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WPP.Units
{
    class SpawnBuilding : Unit
    {
        public float hitpoints;
        public float life_time;
        public int spawn_unit_id;
        public int spawn_unit_count;

        public SpawnBuilding(int id, string name, int level)
            : base(id, name, level)
        {
            hitpoints = 0.0f;
            life_time = 0.0f;
            spawn_unit_id = 0;
            spawn_unit_count = 0;
        }
    }
}