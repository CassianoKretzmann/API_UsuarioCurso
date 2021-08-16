using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Models
{
    public class ValidaCamposViewModelOutput
    {
        public IEnumerable<string> Erros { get; private set; }

        public ValidaCamposViewModelOutput(IEnumerable<string> erros) 
        {
            Erros = erros;
        }
    }
}
