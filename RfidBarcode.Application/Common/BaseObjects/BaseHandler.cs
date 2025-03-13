using AutoMapper;
using RfidBarcode.Application.Common.Interfaces;
using System.Data.Common;
using System.Globalization;

namespace RfidBarcode.Application.Common.BaseObjects
{
    public class BaseHandler
    {
        protected IApplicationDbContext _context { get; set; } = null!;
        protected IMapper _mapper { get; set; } = null!;

        protected CultureInfo mCulture { get; set; } = CultureInfo.InvariantCulture;
        protected int mTimezoneOffset { get; set; } = 0;

        protected void ConfigLocalization(string? language, int? timezoneOffset)
        {
            if (language != null)
            {
                try
                {
                    mCulture = new CultureInfo(language);
                    Thread.CurrentThread.CurrentCulture = mCulture;
                    Thread.CurrentThread.CurrentUICulture = mCulture;
                }
                catch { }
            } 
            else
            {
                mCulture = new CultureInfo("id");
                Thread.CurrentThread.CurrentCulture = mCulture;
                Thread.CurrentThread.CurrentUICulture = mCulture;
            }

            if (timezoneOffset != null)
            {
                mTimezoneOffset = (int)timezoneOffset - 7;
            }
        }

        protected long? GetInt64(DbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetInt64(index);
            }
            return null;
        }

        protected double? GetDouble(DbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetDouble(index);
            }
            return null;
        }


        protected int? GetInt(DbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetInt32(index);
            }
            return null;
        }

        protected string? GetString(DbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetString(index);
            }
            return null;
        }

        protected DateTime? GetDateTime(DbDataReader reader, int index)
        {
            if (!reader.IsDBNull(index))
            {
                return reader.GetDateTime(index);
            }
            return null;
        }
    }
}
