namespace ZnModelModule.Shared.InternalCassandra.Storage
{
    public struct ValueAtDateSequence
    {
        public IEnumerable<ValueAtDate> Sequence { get; }

        public ValueAtDateSequence(IEnumerable<ValueAtDate> sequence)
        {
            ArgumentNullException.ThrowIfNull(sequence);
            this.Sequence = sequence;
        }

        public ValueAtDateSequence MaterializeToArray()
        {
            return new ValueAtDateSequence(Sequence.ToArray());
        }
    }
}
