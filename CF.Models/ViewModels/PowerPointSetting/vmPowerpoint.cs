using System.Collections.ObjectModel;
using CF.Models.DataModels;
using CF.Models.DataModels.Powerpoint;


namespace CF.Models.ViewModels.PowerPointSetting
{
    public partial class vmPowerpoint
    {
        private mPowerpoint _Origin = null;


    }
    public partial class vmPowerpoint : vmBase
    {
        public vmPowerpoint(mPowerpoint origin)
        {
            SetInitialData();

            this.Origin = origin;
            
        }


        public mPowerpoint Origin
        {
            get => _Origin;
            private set
            {
                _Origin = value;
                if (value == null) return;

                foreach (mSlide slide in value.Slides)
                {
                    vmSlide newSlide = new vmSlide(slide);
                    this.Slides.Add(newSlide);  
                }
            }
        }

        
        public ObservableCollection<vmSlide> Slides { get; private set; }



        

    }
    public partial class vmPowerpoint
    {
        public override void SetInitialData()
        {
            this.Slides = new ObservableCollection<vmSlide>();
            this.Slides.CollectionChanged += Slides_CollectionChanged;
        }
        public override void UpdateOriginData()
        {
            foreach (vmSlide slide in this.Slides)
            {
                slide.Origin.ParentId = this.Origin.Id;
                slide.UpdateOriginData();
            }
        }






        private void Slides_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    if (e.NewItems == null) return;
                    foreach (var slide in e.NewItems)
                    {
                        vmSlide newSlide = slide as vmSlide;
                        if (newSlide == null) continue;

                        newSlide.ParentPowerpoint = this;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }
    }

}
