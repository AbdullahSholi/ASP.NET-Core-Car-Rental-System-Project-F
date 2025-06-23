async function fetchData() {
    const response = await fetch('https://fakestoreapi.com/products/1');
    const data = await response.json();
    console.log(data);
    document.getElementById('result').innerText = JSON.stringify(data, null, 2);
}
