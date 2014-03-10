using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;


namespace RF.BL.Model
{
    public class WorkCalendar : BaseModel
    {
        private bool _isWorkingDay;
        public bool IsWorkingDay
        {
            get
            {
                return _isWorkingDay;
            }
            set
            {
                _isWorkingDay = value;
                this.OnPropertyChanged("IsWorkingDay");
            }
        }

        public DateTime Date { get; set; }
        public DateTime BankDate { get; set; }
        public int BankDate_Key { get; set; }

        private string _comment;
        [Required(ErrorMessage = "Требуется коммент")]
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                this.OnPropertyChanged("Comment");
            }
        }

        public string DayType
        {
            get
            {
                return IsWorkingDay ? "рабочий" : "выходной";
            }
        }

        public int Datepart(WorkCalendar wc)
        {
            return 1;
        }
    }
}
