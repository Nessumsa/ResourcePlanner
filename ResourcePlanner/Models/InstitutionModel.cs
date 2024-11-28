using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ResourcePlanner.Models
{
    class InstitutionModel : INotifyPropertyChanged
    {
        public String institutionID = "";
        public String startTime = "";
        public String endTime = "";
        public String timeInterval = "";


        public InstitutionModel(String ID, String startTime, String endTime, String timeInterval)
        {
            this.institutionID = ID;
            this.startTime = startTime;
            this.endTime = endTime;
            this.timeInterval = timeInterval;
        }
        public String InstitutionID
        {
            get => institutionID;
            set
            {
                if (institutionID != value)
                {
                    institutionID = value;
                    OnPropertyChanged(nameof(InstitutionID));
                }
            }
        }
        public String StartTime
        {
            get => startTime;
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    OnPropertyChanged(nameof(StartTime));
                }
            }
        }

        public String EndTime
        {
            get => endTime;
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    OnPropertyChanged(nameof(EndTime));
                }
            }
        }
        public String TimeInterval
        {
            get => timeInterval;
            set
            {
                if (timeInterval != value)
                {
                    timeInterval = value;
                    OnPropertyChanged(nameof(TimeInterval));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
