const API_URL = "http://localhost:5109/api/Product";

let editMode = false;
let editId = null;

async function GetProducts() {
  const container = document.getElementById("product-list");
  container.innerHTML = "";

  try {
    const res = await fetch(API_URL);
    if (!res.ok) {
      throw new Error("Unable to load products.");
    }

    const data = await res.json();
    data.forEach(product => {
      container.appendChild(CreateProductCard(product));
    });
  } catch (error) {
    container.innerHTML = `
      <div class="col-12">
        <div class="alert alert-danger mb-0">${error.message}</div>
      </div>
    `;
  }
}

async function AddProduct() {
  const name = document.getElementById("name").value.trim();
  const priceValue = document.getElementById("price").value;
  const category = document.getElementById("category").value.trim();
  const price = Number(priceValue);

  if (!name || Number.isNaN(price) || price <= 0) {
    alert("Please enter a valid name and price.");
    return;
  }

  const product = {
    id: editId ?? 0,
    name,
    price,
    category
  };

  const requestUrl = editMode ? `${API_URL}/${editId}` : API_URL;
  const requestMethod = editMode ? "PUT" : "POST";

  try {
    const res = await fetch(requestUrl, {
      method: requestMethod,
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(product)
    });

    if (!res.ok) {
      throw new Error(await GetErrorMessage(res, editMode ? "update" : "add"));
    }

    ResetForm();
    await GetProducts();
  } catch (error) {
    alert(error.message);
  }
}

async function DeleteProduct(id) {
  const res = await fetch(`${API_URL}/${id}`, {
    method: "DELETE"
  });

  if (!res.ok) {
    alert("Unable to delete product.");
    return;
  }

  if (editId === id) {
    ResetForm();
  }

  await GetProducts();
}

function EditProduct(product) {
  document.getElementById("name").value = product.name ?? "";
  document.getElementById("price").value = product.price ?? "";
  document.getElementById("category").value = product.category ?? "";

  editMode = true;
  editId = product.id;

  document.getElementById("form-title").innerText = "Edit Product";
  document.getElementById("submit-btn").innerText = "Update";
}

function searchProducts() {
  const value = document.getElementById("search").value.toLowerCase();

  document.querySelectorAll(".product-card").forEach(card => {
    card.parentElement.style.display = card.dataset.searchText.includes(value)
      ? "block"
      : "none";
  });
}

function CreateProductCard(product) {
  const column = document.createElement("div");
  column.className = "col-md-4 mb-3";

  const card = document.createElement("div");
  card.className = "card product-card p-3 mb-3 shadow-sm h-100";
  card.id = product.id;
  card.dataset.searchText = `${product.name ?? ""} ${product.price ?? ""} ${product.category ?? ""}`.toLowerCase();

  const title = document.createElement("h5");
  title.textContent = product.name ?? "";

  const price = document.createElement("p");
  price.textContent = `₹ ${product.price ?? ""}`;

  const category = document.createElement("p");
  category.textContent = product.category ?? "";

  const actions = document.createElement("div");
  actions.className = "d-flex gap-2";

  const deleteButton = document.createElement("button");
  deleteButton.className = "btn btn-danger btn-sm w-50";
  deleteButton.textContent = "Delete";
  deleteButton.addEventListener("click", () => DeleteProduct(product.id));

  const editButton = document.createElement("button");
  editButton.className = "btn btn-warning btn-sm w-50";
  editButton.textContent = "Edit";
  editButton.addEventListener("click", () => EditProduct(product));

  actions.append(deleteButton, editButton);
  card.append(title, price, category, actions);
  column.appendChild(card);

  return column;
}

function ResetForm() {
  document.getElementById("name").value = "";
  document.getElementById("price").value = "";
  document.getElementById("category").value = "";

  editMode = false;
  editId = null;

  document.getElementById("form-title").innerText = "Add Product";
  document.getElementById("submit-btn").innerText = "Add";
}

async function GetErrorMessage(response, action) {
  try {
    const error = await response.json();
    const validationMessages = Object.values(error.errors ?? {}).flat();

    if (validationMessages.length > 0) {
      return validationMessages.join(" ");
    }

    if (typeof error.title === "string" && error.title) {
      return error.title;
    }
  } catch {
    // Ignore JSON parsing failures and fall back to a generic message.
  }

  return `Unable to ${action} product.`;
}

GetProducts();
