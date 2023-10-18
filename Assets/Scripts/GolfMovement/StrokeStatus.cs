namespace Golf
{
	public enum StrokeStatus
	{
		NotTaken,
		Aiming,
		InMotion,
		Taken,//did it.
		SankIt,//ended in the hole after this one.
		Failure// out of bounds; in trap; needs reset.
	}
}