//Bookmarklet function to set a string column data for D365 apps - run on open record form.
javascript: (function () {
  let colSchema = prompt("Enter schema name (e.g. new_name):", "cm_name");
  if (!colSchema) {
    alert("Schema name is required.");
    return;
  }

  let colValue = prompt("Column value to set as string value:", "Don Draper");
  if (!colValue) {
    alert("Column value is required.");
    return;
  }

  let data = {
    [colSchema]: colValue,
  };

  var Id = Xrm.Page.data.entity.getId().replace("{", "").replace("}", "");
  var entityType = Xrm.Page.data.entity.getEntityName();

  Xrm.WebApi.updateRecord(entityType, Id, data).then(
    function success(result) {
      console.log("Success");
    },
    function (error) {
      console.log(error.message);
    }
  );
})();
