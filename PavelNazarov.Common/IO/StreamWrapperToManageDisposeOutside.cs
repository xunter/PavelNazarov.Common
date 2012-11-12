using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PavelNazarov.Common.IO
{
    public class StreamWrapperToManageDisposeOutside : Stream
    {
        private readonly Stream _original;

        public StreamWrapperToManageDisposeOutside(Stream original)
        {
            _original = original;
        }

        public override bool CanRead
        {
            get { return _original.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _original.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _original.CanWrite; }
        }

        public override void Flush()
        {
            _original.Flush();
        }

        public override long Length
        {
            get { return _original.Length; }
        }

        public override long Position
        {
            get
            {
                return _original.Position;
            }
            set
            {
                _original.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _original.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _original.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _original.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _original.Write(buffer, offset, count);
        }

        public void DisposeStreamOutside()
        {
            this.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
 	         //base.Dispose(disposing);
        }
    }
}
