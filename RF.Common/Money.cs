using System;
using System.Text;
using System.Collections.Specialized;

///Slightly modified money-amount-to-string classes  based on the RSDN ones.
///Check url for details: http://rsdn.ru/article/files/dotnet/RusNumber.xml
///Basically, old class is used now as a stub. Some refactoring needed.
namespace RF.Common
{    
    /// <summary>
    /// ��������� ��������� "��������" ��-������ ����� �� 0 �� 999 � ��������� �������� "�������" � ���������������, ������� ��������� � �����. ��������, "�������� ���� �������", �� "��� �������� ��� ������". ��������� ������� � ������� ���.
    /// </summary>
    public class RusNumber
    {
        private static string[] hunds =
            {
                "", "��� ", "������ ", "������ ", "��������� ",
                "������� ", "�������� ", "������� ", "��������� ", "��������� "
            };

        private static string[] tens =
            {
                    "", "������ ", "�������� ", "�������� ", "����� ", "��������� ",
                    "���������� ", "��������� ", "����������� ", "��������� "
            };

        public static string Str(decimal val, bool male, string one, string two, string five)
        {
            string[] frac20 =
                {
                    "", "���� ", "��� ", "��� ", "������ ", "���� ", "����� ",
                    "���� ", "������ ", "������ ", "������ ", "����������� ",
                    "���������� ", "���������� ", "������������ ", "���������� ",
                    "����������� ", "���������� ", "������������ ", "������������ "
                };

            int num = Convert.ToInt32(Math.Truncate(val % 1000));
            if (0 == num) return "";
            if (num < 0) throw new ArgumentOutOfRangeException("val", "�������� �� ����� ���� �������������");
            if (!male)
            {
                frac20[1] = "���� ";
                frac20[2] = "��� ";
            }

            StringBuilder r = new StringBuilder(hunds[num / 100]);

            if (num % 100 < 20)
            {
                r.Append(frac20[num % 100]);
            }
            else
            {
                r.Append(tens[num % 100 / 10]);
                r.Append(frac20[num % 10]);
            }

            r.Append(Case(num, one, two, five));

            if (r.Length != 0) r.Append(" ");
            return r.ToString();
        }

        /// <summary>
        /// ������������ ��� ��������� ���������������� � ����������� �� ����������. ������������ ��� ������������� � ������� RusCurrency � RusNumber.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <param name="five"></param>
        /// <returns></returns>
        public static string Case(int val, string one, string two, string five)
        {
            int t = (val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2:
                case 3:
                case 4: return two;
                default: return five;
            }
        }
    };
    struct CurrencyInfo
    {
        public bool male;
        public string seniorOne, seniorTwo, seniorFive;
        public string juniorOne, juniorTwo, juniorFive;
    };

    /// <summary>
    /// ������������� ����� ��� ����� �����, ���������� �� ������� ������� ������� �� 100 �������. ���� ������ ����������� ������ �������, �������� ����������� ����� �� ������ RusCurrency.
    /// </summary>
    public class RusCurrency
    {
        private static HybridDictionary currencies = new HybridDictionary();

        static RusCurrency()
        {
            Register("RUR", true, "�����", "�����", "������", "�������", "�������", "������");
            Register("KK", true, "�����", "�����", "�����", "����", "����", "����");
            Register("GBP", true, "����", "�����", "������", "����", "�����", "������");
        }
        /// <summary>
        /// ������������ ����� ��� ������. � ������ currency ����������� ����������� ������ (���������� ��� ����� ��� �������). 
        /// �� ���� ������ ������ ���������� �������������� �����. 
        /// �������� male ���������� ��� (������� ��� �������) ������� �������� ������� (��������, ��� ������� male=true). 
        /// ��� ������� ��������� ����� ���������� (� ������ ��������!) ������ �������������� ��� ���� ��������������� ��� ����� 1, 2 � 5 (��������, "������, �������, ��������. ����, �����, ������.").
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="male"></param>
        /// <param name="seniorOne"></param>
        /// <param name="seniorTwo"></param>
        /// <param name="seniorFive"></param>
        /// <param name="juniorOne"></param>
        /// <param name="juniorTwo"></param>
        /// <param name="juniorFive"></param>
        public static void Register(string currency, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            CurrencyInfo info;
            info.male = male;
            info.seniorOne = seniorOne; info.seniorTwo = seniorTwo; info.seniorFive = seniorFive;
            info.juniorOne = juniorOne; info.juniorTwo = juniorTwo; info.juniorFive = juniorFive;
            currencies.Add(currency, info);
        }
        /// <summary>
        /// ���������� ��� ����� val ��������� �������������� ������ � ������.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Str(decimal val)
        {
            return Str(val, "RUR");
        }
        /// <summary>
        /// ������� ������ � ������������ �������� ������� (� ������ ���� ������ ����������������)
        /// </summary>
        /// <param name="val"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string Str(decimal val, string currency)
        {
            if (!currencies.Contains(currency))
                throw new ArgumentOutOfRangeException("currency", "������ \"" + currency + "\" �� ����������������");

            CurrencyInfo info = (CurrencyInfo)currencies[currency];
            return Str(val, info.male,
                info.seniorOne, info.seniorTwo, info.seniorFive,
                info.juniorOne, info.juniorTwo, info.juniorFive);
        }
        /// <summary>
        /// ������� ������ � ������������ �������� ������� (��� ��������������� �����������). �� ���� ������ ������ ���������� �������������� �����. �������� male ���������� ��� (������� ��� �������) ������� �������� ������� (��������, ��� ������� male=true). ��� ������� ��������� ����� ���������� (� ������ ��������!) ������ �������������� ��� ���� ��������������� ��� ����� 1, 2 � 5 (��������, "������, �������, ��������. ����, �����, ������.").
        /// </summary>
        /// <param name="val"></param>
        /// <param name="male"></param>
        /// <param name="seniorOne"></param>
        /// <param name="seniorTwo"></param>
        /// <param name="seniorFive"></param>
        /// <param name="juniorOne"></param>
        /// <param name="juniorTwo"></param>
        /// <param name="juniorFive"></param>
        /// <returns></returns>
        public static string Str(decimal val, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            bool minus = false;
            if (val < 0) { val = -val; minus = true; }

            decimal n = Math.Truncate(val);
            int remainder = Convert.ToInt32(Math.Truncate((val - n + 0.005M) * 100));

            StringBuilder r = new StringBuilder();

            if (0 == n) r.Append("���� ");
            if (n % 1000 != 0)
                r.Append(RusNumber.Str(n, male, seniorOne, seniorTwo, seniorFive));
            else
                r.Append(seniorFive);

            n /= 1000;

            r.Insert(0, RusNumber.Str(n, false, "������", "������", "�����"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "�������", "��������", "���������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "���������", "����������", "�����������"));
            if (minus) r.Insert(0, "����� ");

            r.Append(remainder.ToString(" 0 "));
            r.Append(RusNumber.Case(remainder, juniorOne, juniorTwo, juniorFive));

            return r.ToString().Replace("  ", " ");
        }
    };    
    public static class Money
    {
        #region Constants

        public const int MantissaMoneyDigits = 2;

        #endregion

        #region Public Methods

        public static string MoneyToTextString(this decimal moneyValue)
        {
            return GetLiteralSum(moneyValue);
        }

        public static string GetLiteralSum(decimal val)
        {
            return RusCurrency.Str(val, "RUR");
        }

        public static string GetLiteralSumKK(decimal val)
        {
            return RusCurrency.Str(val, "KK");
        }

        public static decimal CeilingMoney(decimal money)
        {
            return Math.Ceiling(100m * money) / 100m;
        }

        public static decimal FloorMoney(decimal money)
        {
            return Math.Floor(100m * money) / 100m;
        }

        public static void CheckMoney(decimal d)
        {
            if (RoundMoney(d) != d)
                throw new ArgumentException(string.Format("�������� ����� �������� ����� ��� {0} ����� ����� �������: {1}", MantissaMoneyDigits, d));
        }

		/// <summary>
		/// ��������� �������� ����� �������� ��������, ����������� � ������� (�� 2-�� ����� ����� �������)
		/// </summary>
		/// <param name="money">�������� ��� ����������</param>
		/// <returns>����������� �����</returns>
		public static decimal RoundMoney(decimal money)
		{
			return Math.Round(money, MantissaMoneyDigits);
		}

        #endregion
    }
}
