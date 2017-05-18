## EntityModelNameFormatter

In Visual Studio (2017) the Entity Data Model Wizard (CF) generates model with names matching the database format. 
This tool re-formats the names from "lowcase_underscore" to Pascal/Camel format.

For example, an original class may look like this:

```
    [Table("myschema.admin_role")]
    public partial class admin_role
    {
        public admin_role()
        {
            admin_role_function_map = new HashSet<admin_role_function_map>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string role_name { get; set; }

        public virtual ICollection<admin_role_function_map> admin_role_function_map { get; set; }
    }
```

And will be transformed to this (along with the file name):

```
    [Table("myschema.admin_role")]
    public partial class AdminRole
    {
        public AdminRole()
        {
            AdminRoleFunctionMap = new HashSet<AdminRoleFunctionMap>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public virtual ICollection<AdminRoleFunctionMap> AdminRoleFunctionMap { get; set; }
    }
```


## Run tool:
EntityModelNameFormatter SOURCE_FOLDER TARGET_FOLDER
As result the TARGET_FOLDER is deleted and recreated.
