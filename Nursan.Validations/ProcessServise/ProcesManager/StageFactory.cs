using Nursan.Domain.Entity;
using Nursan.Validations.ProcessServise.ProcessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Validations.ProcessServise.ProcesManager
{
    public static class StageFactory
    {

        public static IBarcodeState CreateStage(string stageName)
        {
            // Взимаме пълното име на класа, предполагайки, че класовете за етапите са в текущото пространство от имена
            var type = Type.GetType($"NamespaceOfStages.{stageName}Stage");
            if (type == null)
            {
                throw new ArgumentException($"Неизвестен етап: {stageName}");
            }

            return (IBarcodeState)Activator.CreateInstance(type);
        }

    }
}
