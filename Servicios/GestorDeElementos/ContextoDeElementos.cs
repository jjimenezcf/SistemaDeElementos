﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using GestorDeElementos.BdModelo;
using System.Linq;

namespace GestorDeElementos
{
    public class ContextoDeElementos: DbContext
    {
        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {
        }
        
    }
}
