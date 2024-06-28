import type { BranchProduct } from "$lib/models/BranchProduct";

const API_BASE_URL ='https://localhost:7086/api/v1';

export async function fetchBranchProducts(): Promise<BranchProduct[]> {
  const response = await fetch(`${API_BASE_URL}/BranchProduct`);
  if (!response.ok) {
    throw new Error('Failed to fetch products');
  }
  return await response.json();
}

export async function assignProductToBranch(branchName: string, productName: string): Promise<void> {
  const url = `${API_BASE_URL}/BranchProduct`;
  const requestOptions = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ branchName, productName }),
  };

  const response = await fetch(url, requestOptions);
  if (!response.ok) {
    throw new Error('Failed to assign product to branch');
  }
}

  export async function deleteBranchProduct(id: number): Promise<void> {
    const url = `${API_BASE_URL}/BranchProduct/${id}`;
    const requestOptions = {
      method: 'DELETE',
    };
  
    const response = await fetch(url, requestOptions);
    if (!response.ok) {
      throw new Error('Failed to delete branch product');
    }
  }