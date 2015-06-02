using System.Linq;
using System.Timers;
using Seq.Apps;
using Seq.Apps.LogEvents;

namespace Seq.App.EventFieldDigest
{
    [SeqApp("EventFieldDigest", Description = "Creates a digest event of a event types unique field value for a defined time intervall.")]
    public class EventFieldDigestReactor : Reactor, ISubscribeTo<LogEventData>
    {
        private Timer _timer;
        private Digest _digest;
        
        [SeqAppSetting(DisplayName = "Field name", HelpText = "Field name to digest values from")]
        public string FieldName { get; set; }

        [SeqAppSetting(DisplayName = "Interval", HelpText = "How many hours should event field data be collected before digest is generated", InputType = SettingInputType.Integer)]
        public int Interval { get; set; }

        [SeqAppSetting(DisplayName = "Ignore pattern", HelpText = "Field values that match this pattern will be ignored", IsOptional = true)]
        public string IgnorePattern { get; set; }

        [SeqAppSetting(DisplayName = "Sanitize pattern", HelpText = "Parts of field value that match this pattern will be removed", IsOptional = true)]
        public string SanitizePattern { get; set; }

        [SeqAppSetting(DisplayName = "Stringify digest", HelpText = "Save the Digest filed in the event as a string instead of an object", IsOptional = true)]
        public bool StringifyDigest { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            _digest = new Digest(IgnorePattern, SanitizePattern);
            _timer = new Timer(1000 * 3600 * Interval);
            _timer.Elapsed += CreateDigestEvent;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public async void On(Event<LogEventData> evt)
        {
            if (evt.Data.Properties.ContainsKey(FieldName))
            {
                _digest.Add(evt.Data.Properties[FieldName].ToString());
            }
        }

        private void CreateDigestEvent(object obj, ElapsedEventArgs args)
        {
            _timer.Stop();
            var data = _digest.GetDigest().ToArray();
            _digest.Clear();

            if (data.Any())
            {
                if (StringifyDigest)
                {
                    Log
                    .ForContext("Digest", string.Join("\n", data.Select(e => string.Join(": ", e.Count, e.Url))))
                    .ForContext("FieldName", FieldName)
                    .Information("Field digest for {0}", FieldName);
                }
                else
                {
                    Log
                    .ForContext("Digest", data, true)
                    .ForContext("FieldName", FieldName)
                    .Information("Field digest for {0}", FieldName);
                }
                
            }

            _timer.Start();
        }
    }
}
