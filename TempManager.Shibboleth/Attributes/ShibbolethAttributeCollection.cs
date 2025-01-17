namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kolekce uchovávající atributy Shibboleth.
	/// </summary>
	public class ShibbolethAttributeCollection : HashSet<string>, IShibbolethAttributeCollection
	{
		public ShibbolethAttributeCollection()
		{
		}

		public ShibbolethAttributeCollection(ICollection<string> attributes) : base(attributes)
		{
		}

		/// <summary>
		/// Vrací základních pár atributů - takový minimum pro napojení.
		/// </summary>
		public static ShibbolethAttributeCollection DefaultAttributes = new ShibbolethAttributeCollection(new List<string> {
			"givenName",
			"sn",
			"mail",
			"affiliation",
			"uid",
			"eppn"
		});
	}
}