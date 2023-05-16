let sceltaUtente = prompt("show, create, delete");

if (sceltaUtente === "show")
{
    // get pizza
    let ricercaUtente = prompt("Inserisci");
    axios.get(`https://localhost:7106/api/pizzas/stringsearch/?word=${ricercaUtente ?? ricercaUtente}`)
        .then(res => {
            console.log(res);
        })
}

if (sceltaUtente === "create")
{
    let nome = prompt("Inserisci nome pizza");
    let descrizione = prompt("Inserisci descrizione pizza");
    let prezzo = prompt("Inserisci prezzo pizza");
    let img = prompt("Inserisci img pizza");
    let categoryId = prompt("Inserisci categoryId pizza");
    let ingredients = prompt("Inserisci ingredients pizza");

    axios.post("https://localhost:7106/api/pizzas/create", {
    nome: nome,
        descrizione: descrizione,
        prezzo: prezzo,
        img: img,
        categoryId: categoryId,
        ingredients: [{ "id": ingredients }]
    })
        .then(res => {
             console.log(res);
         })
}

if (sceltaUtente === "delete")
{
    let idDelete = prompt("Id da eliminare");

    axios.delete(`https://localhost:7106/api/pizzas/delete/${idDelete}`)
        .then(res => {
            console.log(res);
        });
}


