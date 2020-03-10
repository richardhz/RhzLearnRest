using System;
using System.Collections.Generic;
using System.Text;

namespace RhzLearnRest.Domains.Interfaces
{
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
