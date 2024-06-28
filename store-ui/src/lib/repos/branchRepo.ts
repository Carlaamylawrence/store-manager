
import type { Branch } from '$lib/models/Branch';

const API_BASE_URL ='https://localhost:7086/api/v1';

export async function fetchBranches(): Promise<Branch[]> {
  const response = await fetch(`${API_BASE_URL}/branches`);
  if (!response.ok) {
    throw new Error('Failed to fetch products');
  }
  return await response.json();
}

export async function fetchBranch(id: number): Promise<Branch> {
  const response = await fetch(`${API_BASE_URL}/branches/${id}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch branch with id ${id}`);
  }
  return await response.json();
}

export async function addBranch(branch: Branch): Promise<Branch> {
  const response = await fetch(`${API_BASE_URL}/branches`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(branch),
  });
  if (!response.ok) {
    throw new Error('Failed to create branch');
  }
  return branch;
}

export async function updateBranch(branch : Branch): Promise<Branch> {
  const response = await fetch(`${API_BASE_URL}/branches/${branch.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(branch)
  });
  if (!response.ok) {
    throw new Error(`Failed to update ${branch.name}`);
  }
  return branch;
}

export async function deleteBranch(id: number): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/branches/${id}`, {
    method: 'DELETE'
  });
  if (!response.ok) {
    throw new Error(`Failed to delete branch`);
  }
}
