using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class ResourceAutoLink
{
    public string resource_name_alias { get; set; }
    public string resource_name_db { get; set; }
}

public class ResourceAutoLinks : List<ResourceAutoLink>
{
    public ResourceAutoLinks()
    {
        Add(new ResourceAutoLink { resource_name_db = "is_active", resource_name_alias = "Active" });
        Add(new ResourceAutoLink { resource_name_db = "create_by", resource_name_alias = "Create By" });
        Add(new ResourceAutoLink { resource_name_db = "create_date", resource_name_alias = "Create Date" });
        Add(new ResourceAutoLink { resource_name_db = "update_by", resource_name_alias = "Update By" });
        Add(new ResourceAutoLink { resource_name_db = "update_date", resource_name_alias = "Update Date" });
    }
}