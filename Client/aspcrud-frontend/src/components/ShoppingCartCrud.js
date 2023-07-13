import React from 'react';
import axios from 'axios';
import './styles.css';
import { useEffect, useState } from "react";
import { urlNewProduct,urlProducts, urlDeleteProduct } from '../endpoints';

function ShoppingCartCrud() {

  const [name, setName] = useState("");
  const [price, setPrice] = useState(0);
  const [amount, setAmount] = useState(0);
  const [total, setTotal] = useState(0);
  const [products, setProducts] = useState([]);
  const [priceLimit, setPriceLimit] = useState(0);

  useEffect(() => {
    (async () => await Load())();
  }, []);

  let sum = 0;
  async function Load() {
    const result = await axios.get(urlProducts);
    setProducts(result.data);
    calcTotalAmount(result.data);
  }

  function calcTotalAmount(productResult) {
    sum = 0;
    setTotal(0);
    productResult.forEach(totalAmount);
    setTotal(sum);
    if (priceLimit > 0 && priceLimit < sum) {
      alert("You passed the max limit of " + priceLimit);
    }
  }

  function totalAmount(product) {
    sum += (product.amount * product.price);
  }

  const handlePriceLimitChange = (e) => {
    setPriceLimit(e.target.value);
  };

  async function save(event) {
    event.preventDefault();

    await axios.post(urlNewProduct, {
      name: name,
      price: price,
      amount: amount,
    }).then(response => alert('Product added Successfully'))
      .catch(error => {
        alert('Failed to add product')
        console.error('Failed to add product!', error);
      });
    setName("");
    setPrice(0);
    setAmount(0);
    Load();
  }


  async function DeleteProduct(name) {
    await axios.delete(urlDeleteProduct + name)
      .then(response => alert('Product deleted successful'))
      .catch(error => {
        alert('Failed to delete product')
        console.error('Failed to delete product!', error);
      });
    setName("");
    setPrice(0);
    setAmount(0);
    Load();
  }


  return (
    <div>
      <h1>Shopping Cart Details</h1>
      <div>
        <form>
          <div>
            <div><label>Product Name</label></div>
            <div><input
              type="text"
              id="name"
              value={name}
              onChange={(event) => {
                setName(event.target.value);
              }}
            /></div>
          </div>
          <div className="form-group">
            <div><label>Price</label></div>
            <div><input
              type="text"
              className="form-control"
              id="price"
              value={price}
              onChange={(event) => {
                setPrice(event.target.value);
              }}
            /></div>
          </div>

          <div>
            <div><label>Amount</label></div>
            <div ><input
              type="text"
              className="form-control"
              id="amount"
              value={amount}
              onChange={(event) => {
                setAmount(event.target.value);
              }}
            /></div>
          </div>

          <div>
            <button onClick={save}>
              Add
            </button>
          </div>
        </form>
      </div>
      <br></br>

      <table align="center">
        <thead>
          <tr>
            <th scope="col">Picked Up</th>
            <th scope="col">Product Name</th>
            <th scope="col">Price</th>
            <th scope="col">Amount</th>
            <th scope="col">Option</th>
          </tr>
        </thead>

        {products.map(function fn(product) {
          return (
            <tbody>
              <tr key={product.name}>
                <td><input type="checkbox" /></td>
                <td>{product.name} </td>
                <td>{product.price}</td>
                <td>{product.amount}</td>
                <td>
                  <button
                    type="button"
                    onClick={() => DeleteProduct(product.name)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            </tbody>
          );
        })}
      </table>
      <div>
        <label><h3>Price Limit: </h3></label>
        <input
          type="text"
          value={priceLimit}
          onChange={handlePriceLimitChange}
        />
      </div>
      <label><h3>Total cost of all the products: {total}</h3></label>
    </div>
  );
}

export default ShoppingCartCrud;
