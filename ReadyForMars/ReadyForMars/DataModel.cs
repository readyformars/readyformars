using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace ReadyForMars
{
    public class DataRetriever
    {
        private RecordMetadata _lastData;
        private String _urlTemplate = "http://marsweather.ingenology.com/v1/archive/?page={0}";
        private Int32 _nextPageNum = 1;
        private Int32 _currentPage = 1;

        public event EventHandler<DataRetrieverEventArgs> DataRetrieveCompleted;

        public Int32 CurrentPage
        {
            get { return _currentPage; }
        }

        public RecordMetadata LastData
        {
            get { return _lastData; }
        }

        public DataRetriever()
        {
            _lastData = null;
        }

        /// <summary>
        /// Starts an asynchronous request for the next page only after a call to GetFirstPage.
        /// </summary>
        /// <returns>false if there is not another page, or GetFirstPage hasn't been called before</returns>
        public bool GetNextPage()
        {
            if (_lastData != null && _lastData.next != null)
            {
                //await UpdateData();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Starts an asynchronous request for the first page
        /// </summary>
        public async Task<RecordMetadata> GetFirstPage()
        {
            this._lastData = null;
            this._nextPageNum = 1;
            return await UpdateData();
        }

        async Task<RecordMetadata> UpdateData()
        {
            String request = String.Format(this._urlTemplate, this._nextPageNum);
            
            // Create an HttpClient instance 
            HttpClient client = new HttpClient();

            String urlContents = await client.GetStringAsync(request);
            this._lastData = JsonConvert.DeserializeObject<RecordMetadata>(urlContents);
            this._currentPage = this._nextPageNum;
            this._nextPageNum++;
            //OnRaiseCustomEvent(new DataRetrieverEventArgs(this._lastData));

            return this._lastData;

            /*

            // Send a request asynchronously continue when complete 
            client.GetAsync(request).ContinueWith(
                (requestTask) =>
                {
                    // Get HTTP response from completed task. 
                    HttpResponseMessage response = requestTask.Result;

                    // Check that response was successful or throw exception 
                    //response.EnsureSuccessStatusCode();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        // Read response asynchronously as JsonValue and write out top facts for each country 
                        response.Content.ReadAsStringAsync().ContinueWith(
                            (task) =>
                            {
                                if (task.Exception == null)
                                {
                                    if (!task.IsCanceled)
                                    {
                                        this._lastData = JsonConvert.DeserializeObject<RecordMetadata>(task.Result);
                                        this._currentPage = this._nextPageNum;
                                        this._nextPageNum++;
                                        OnRaiseCustomEvent(new DataRetrieverEventArgs(this._lastData));
                                    }

                                }
                            }
                        );
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        Console.WriteLine("re-try");

                        this._nextPageNum++;
                        UpdateData();
                    }
                });
            */
            //_wc.DownloadDataAsync(new Uri(request));
        }

        protected virtual void OnRaiseCustomEvent(DataRetrieverEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of a race condition
            EventHandler<DataRetrieverEventArgs> handler = DataRetrieveCompleted;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class DataRetrieverEventArgs : EventArgs
    {
        private RecordMetadata rm;

        public DataRetrieverEventArgs(RecordMetadata rm)
        {
            this.rm = rm;
        }

        public RecordMetadata recordMetadata {
            get { return this.rm; }
        }
    }

    public class Record
    {
        public DateTime terrestrial_date;
        public int sol;
        public float? ls;
        public float? min_temp;
        public float? max_temp;
        public float? pressure;
        public String pressure_string;
        public String abs_humidity;
        public float? wind_speed;
        public String wind_direction;
        public String atmo_opacity;
        public String season;
        public String sunrise;
        public String sunset;
    }
    
    public class RecordMetadata
    {
        public int count;
        public String next;
        public String previous;
        public Record[] results;
    }
}
