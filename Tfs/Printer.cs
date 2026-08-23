using System.Text;
using Yarkov.Tfs.Models;

namespace Yarkov.Tfs.Utilities;

public static class Printer
{
	public static string Print(object entity)
	{
		ArgumentNullException.ThrowIfNull(entity);

		var sb = new StringBuilder();
		var dictionaries = new List<string> {"_links", "Fields", "RevisionFields", "ResourceContainers"};
		foreach (var property in entity.GetType().GetProperties())
		{
			if (dictionaries.Contains(property.Name))
			{
				var value = property.GetValue(entity);
				if (value == null)
				{
					continue;
				}
				
				if (property.Name == "_links")
				{
					foreach (var item in value as Dictionary<string, Link>)
					{
						sb.AppendLine(
$$"""
{{property.Name}}: {
{{item.Key}}: {
{{item.Value}}
}
}
""");
					}
				}
				else if (property.Name == "Fields")
				{
					if (value as Dictionary<string, FieldChange> != null)
					{
						foreach (var item in value as Dictionary<string, FieldChange>)
						{
							sb.AppendLine(
$$"""
{{property.Name}}: {
{{item.Key}}: {
{{item.Value}}
}
}
""");
						}
					}
					else
					{
						foreach (var item in value as Dictionary<string, object>)
						{
							sb.AppendLine(
$$"""
{{property.Name}}: {
{{item.Key}}: {
{{item.Value}}
}
}
""");
						}
					}
				}
				else if (property.Name == "RevisionFields")
				{
					foreach (var item in value as Dictionary<string, object>)
					{
						sb.AppendLine(
$$"""
{{property.Name}}: {
{{item.Key}}: {
{{item.Value}}
}
}
""");
					}
				}
				else if (property.Name == "ResourceContainers")
				{
					foreach (var item in value as Dictionary<string, ResourceContainer>)
					{
						sb.AppendLine(
$$"""
{{property.Name}}: {
{{item.Key}}: {
{{item.Value}}
}
}
""");
					}
				}
			}
			else
			{
				sb.AppendLine($"{property.Name}: {property.GetValue(entity)}");
			}
		}
		return sb.ToString();
	}
}