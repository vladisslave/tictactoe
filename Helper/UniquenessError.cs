class UniquenessError
{
    private string errorDetails;

    public string ErrorDetails { get { return errorDetails; } }
    public UniquenessError(string errorDetails)
    {
        this.errorDetails = errorDetails;
    }
}