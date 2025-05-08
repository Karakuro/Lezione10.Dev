document.addEventListener("DOMContentLoaded", () => { GetData(); });
const baseUrl = "https://localhost:7259"
let template_studs = (item) => {
    return `<tr>
            <td>${item.id}</td>
            <td>${item.name}</td>
            <td>${item.surname}</td>
      </tr>`
};

let GetData = () => {
    return fetch(`${baseUrl}/api/student`)
    .then(result => result.json())
    .then(json => InsertDataInTable(json));
}

let InsertDataInTable = (json) => {
    let rows = json.map(template_studs).join('');
    document.getElementById('table_body').innerHTML = rows;
}