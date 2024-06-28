
import type { Product } from '$lib/models/Product';

const API_BASE_URL = 'https://localhost:7086/api/v1';

export async function fetchProducts(): Promise<Product[]> {
  const response = await fetch(`${API_BASE_URL}/products`);
  if (!response.ok) {
    throw new Error('Failed to fetch products');
  }
  return await response.json();
}

export async function fetchProduct(id: number): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/products/${id}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch product with id ${id}`);
  }
  return await response.json();
}

export async function addProduct(product: Product): Promise<Product> {
  const response = await fetch(`${API_BASE_URL}/products`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(product),
  });
  if (!response.ok) {
    throw new Error('Failed to add product');
  }
  return product;
}

export async function updateProduct(product: Product): Promise<Product> {
  const response = await fetch(`${API_BASE_URL}/products/${product.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(product),
  });

  if (!response.ok) {
    throw new Error(`Failed to update ${product.name}`);
  }
  return product;
}


export async function deleteProduct(id: number): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/products/${id}`, {
    method: 'DELETE',
  });
  if (!response.ok) {
    throw new Error('Failed to delete product');
  }
}
