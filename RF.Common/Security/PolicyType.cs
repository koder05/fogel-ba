using System;
using System.ComponentModel;

namespace RF.Common.Security
{
	/// <summary>
	/// ���� �������. ������������ ��� ���������� ������� �������.
	/// </summary>
    public enum PolicyType
    {
        [Description("�� �����")]
        NotSet = 0,

		/// <summary>
		/// �������� ������� ���� ������������ ������ � �������
		/// </summary>
        [Description("�������� ��� �������")]
        Reports = 1,

        [Description("�������� ��� �����")]
        Services = 2
    }
}