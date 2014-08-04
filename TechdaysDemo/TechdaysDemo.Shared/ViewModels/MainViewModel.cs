using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using ReactiveUI;
using TechdaysDemo.Models;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TechdaysDemo.ViewModels
{
    public class MainViewModel : ReactiveObject, ISupportsActivation
    {
        public ViewModelActivator Activator { get; protected set; }

        public MainViewModel()
        {
            var whenAnyColorChanges = this.WhenAny(x => x.Red, x => x.Green, x => x.Blue,
                (r, g, b) => IntegersToColor(r.Value, g.Value, b.Value));

            Activator = new ViewModelActivator();

            this.WhenActivated(d =>
            {
                //create color property
                d(whenAnyColorChanges
                    .Where(c => c != null)
                    .Select(c => new SolidColorBrush(c.Value))
                    .ToProperty(this, x => x.FinalColor, out this.color));

                // create the command
                d(this.SearchCommand = ReactiveCommand.CreateAsyncObservable(
                    whenAnyColorChanges.Select(x => x != null),
                    c => this.GetImages(c as SolidColorBrush).Select(e => e)));

                // subscribe to the result of the command
                d(this.SearchCommand.Select(ie => new ReactiveList<string>(ie)).ToProperty(this, x => x.Images, out this.images));
            });            
        }

        private ReactiveCommand<IEnumerable<string>> searchCommand;
        public ReactiveCommand<IEnumerable<string>> SearchCommand
        {
            get { return this.searchCommand; }
            private set { this.RaiseAndSetIfChanged(ref this.searchCommand, value); }
        }

        private int red;
        public int Red
        {
            get { return red; }
            set { this.RaiseAndSetIfChanged(ref red, value); }
        }


        private int green;
        public int Green
        {
            get { return green; }
            set { this.RaiseAndSetIfChanged(ref green, value); }
        }


        private int blue;
        public int Blue
        {
            get { return blue; }
            set { this.RaiseAndSetIfChanged(ref blue, value); }
        }


        private ObservableAsPropertyHelper<SolidColorBrush> color;
        public SolidColorBrush FinalColor
        {
            get { return color.Value; }
        }

        private ObservableAsPropertyHelper<ReactiveList<string>> images;
        public ReactiveList<string> Images
        {
            get { return this.images.Value; }
        }

        private Color? IntegersToColor(int red, int green, int blue)
        {
            byte? r = red < 0 || red > 255 ? (byte?)null : (byte?)red;
            byte? g = green < 0 || green > 255 ? (byte?)null : (byte?)green;
            byte? b = blue < 0 || blue > 255 ? (byte?)null : (byte?)blue;


            if (r == null || g == null || b == null) return null;
            return Color.FromArgb(0xFF, r.Value, g.Value, b.Value);
        }

        private IObservable<IEnumerable<string>> GetImages(SolidColorBrush color)
        {
            if(color == null)
            {
                return Observable.Return(new string[0]);
            }

            string url = string.Format("http://labs.tineye.com/rest/?method=flickr_color_search&limit=73&offset=0&colors[0]={0:x2}{1:x2}{2:x2}&weights[0]=1", color.Color.R, color.Color.G, color.Color.B);

            HttpClient http = new HttpClient();

            return http.GetStringAsync(url)
                .ToObservable()
                .Catch<string, Exception>(e => Observable.Empty<string>())
                .Select(str => JsonConvert.DeserializeObject<ImageList>(str).result)
                .Select(images => images.Select(image => "http://img.tineye.com/flickr-images/?filepath=labs-flickr/" + image.filepath));
        }
    }
}
