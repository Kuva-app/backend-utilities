using System.Linq;

namespace Utilities.General.Validations
{
    public static class DocumentValidation
    {
        /// <summary>
        /// Algoritimo de validação do cnpj.
        /// </summary>
        /// <param name="document">CNPJ</param>
        /// <returns>Retorna se o valor definido é válido.</returns>
        public static bool IsCnpj(string document)
        {
            int[] arrMultiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] arrMultiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            string[] arrInvalidos = ["00000000000000", "11111111111111", "22222222222222", "33333333333333", "44444444444444",
                "55555555555555", "66666666666666", "77777777777777", "88888888888888", "99999999999999"];
            int intSoma;
            int intResto;
            string strDigito;
            string strTempCNPJ;
            if (arrInvalidos.Contains(document)) return false;
            document = document.Trim();
            document = document.Replace(".", "").Replace("-", "").Replace("/", "");
            if (document.Length != 14)
            {
                return false;
            }
            strTempCNPJ = document.Substring(0, 12);
            intSoma = 0;
            for (var i = 0; i < 12; i++)
            {
                intSoma += int.Parse(strTempCNPJ[i].ToString()) * arrMultiplicador1[i];
            }
            intResto = intSoma % 11;
            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }
            strDigito = intResto.ToString();
            strTempCNPJ = strTempCNPJ + strDigito;
            intSoma = 0;
            for (var i = 0; i < 13; i++)
            {
                intSoma += int.Parse(strTempCNPJ[i].ToString()) * arrMultiplicador2[i];
            }

            intResto = intSoma % 11;
            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }
            strDigito = $"{strDigito}{intResto.ToString()}";
            return document.EndsWith(strDigito);
        }

        /// <summary>
        /// Algoritimo de validação do cpf.
        /// </summary>
        /// <param name="document">CPF</param>
        /// <returns>Retorna se o valor definido é válido.</returns>
        public static bool IsCpf(string document)
        {
            int[] arrMultiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] arrMultiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
            string[] arrInvalidos = ["00000000000", "11111111111", "22222222222", "33333333333", "44444444444",
                "55555555555", "66666666666", "77777777777", "88888888888", "99999999999"];
            string strTempCPF;
            string strDigito;
            int intSoma;
            int intResto;
            if (arrInvalidos.Contains(document)) return false;
            document = document.Trim();
            document = document.Replace(".", "").Replace("-", "");
            if (document.Length != 11)
            {
                return false;
            }
            strTempCPF = document.Substring(0, 9);
            intSoma = 0;
            for (var i = 0; i < 9; i++)
            {
                intSoma += int.Parse(strTempCPF[i].ToString()) * arrMultiplicador1[i];
            }
            intResto = intSoma % 11;
            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }
            strDigito = intResto.ToString();
            strTempCPF = strTempCPF + strDigito;
            intSoma = 0;
            for (var i = 0; i < 10; i++)
            {
                intSoma += int.Parse(strTempCPF[i].ToString()) * arrMultiplicador2[i];
            }
            intResto = intSoma % 11;
            if (intResto < 2)
            {
                intResto = 0;
            }
            else
            {
                intResto = 11 - intResto;
            }
            strDigito = $"{strDigito}{intResto.ToString()}";
            return document.EndsWith(strDigito);
        }
    }
}