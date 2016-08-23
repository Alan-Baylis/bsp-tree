using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BspTree.Import
{
    public abstract class ImportBuilder
    {
        /// <summary>
        /// Creates new instance of ImportBuilder with explicit return type
        /// </summary>
        public static ImportBuilder<TReturn> Create<TReturn>()
        {
            return new ImportBuilder<TReturn>();
        }

        /// <summary>
        /// Creates new instance of ImportBuilder with implicit return type based on the type of object.
        /// Suitable for anonymous types
        /// </summary>
        public static ImportBuilder<TReturn> Create<TReturn>(TReturn obj)
        {
            return new ImportBuilder<TReturn>();
        }
    }

    public class ImportBuilder<TReturn> : ImportBuilder
    {
        #region Fields
        private string _filepath;
        private bool _embedded;
        #endregion

        #region Methods
        /// <summary>
        /// Sets filepath
        /// </summary>
        public ImportBuilder<TReturn> ReadFrom(string filepath)
        {
            this._filepath = filepath;
            return this;
        }

        /// <summary>
        /// Marks that the source file is embedded resource
        /// </summary>
        public ImportBuilder<TReturn> Embedded()
        {
            this._embedded = true;
            return this;
        }

        /// <summary>
        /// Reads data from file and convert it to required representation
        /// </summary>
        public TReturn Read()
        {
            var resultText = string.Empty;

            if (this._embedded)
            {
                var assembly = Assembly.GetExecutingAssembly();

                using (var stream = assembly.GetManifestResourceStream(this._filepath))
                using (var reader = new StreamReader(stream))
                {
                    resultText = reader.ReadToEnd();
                }
            }
            else
            {
                resultText = File.ReadAllText(this._filepath);
            }


            return (TReturn)JsonConvert.DeserializeObject(resultText, typeof(TReturn));
        }
        #endregion
    }
}
