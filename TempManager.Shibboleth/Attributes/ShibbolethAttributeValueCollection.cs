namespace TempManager.Shibboleth
{
	/// <summary>
	/// Kolekce uchovávající Shibboleth atributy podle jejich identifikátorů.
	/// </summary>
	public class ShibbolethAttributeValueCollection : Dictionary<string, ShibbolethAttributeValue>
	{
		public ShibbolethAttributeValueCollection()
		{
		}

		public ShibbolethAttributeValueCollection(ShibbolethAttributeValueCollection collection) : base(collection)
		{
		}

		public ShibbolethAttributeValueCollection(IDictionary<string, ShibbolethAttributeValue> collection) : base(collection)
		{
		}

		/// <summary>
		/// Přidá attribute do kolekce.
		/// </summary>
		/// <param name="attributeValue"></param>
		public void Add(ShibbolethAttributeValue attributeValue)
		{
			Add(attributeValue.Id, attributeValue);
		}

		/// <summary>
		/// Vrací všechny identifikátory atributů.
		/// </summary>
		public KeyCollection AttributeIds => this.Keys;

		/// <summary>
		/// Vrací, zda-li kolekce obsahuje předaný atribut.
		/// </summary>
		public bool ContainsAttribute(string attributeId)
		{
			return ContainsKey(attributeId);
		}

		/// <summary>
		/// Vrací, zda-li hodnota atributu je prázdná (nebo null).
		/// </summary>
		public bool ValueIsNullOrEmpty(string attributeId)
		{
			return string.IsNullOrEmpty(this[attributeId].Value);
		}
	}
}