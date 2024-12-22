namespace TempManager.Shibboleth
{
	/// <summary>
	/// Hodnota Shibboleth atributu.
	/// </summary>
	public class ShibbolethAttributeValue
	{
		private readonly string id;
		private readonly string value;

		public ShibbolethAttributeValue(string id, string value)
		{
			this.id = id;
			this.value = value;
		}

		/// <summary>
		/// Vrací identifikátor atributu.
		/// </summary>
		public string Id => this.id;

		/// <summary>
		/// Vrací hodnotu atributu.
		/// </summary>
		public string Value => this.value;

		/// <inheritdoc cref="ToString"/>.
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Value))
				return this.Value;
			else
				return base.ToString();
		}
	}
}