<script lang="ts">
  import { onMount } from 'svelte';
  import { fetchBranchProducts, assignProductToBranch, deleteBranchProduct } from '$lib/repos/branchProductRepo';
  import { fetchBranches} from '$lib/repos/branchRepo';
  import { fetchProducts} from '$lib/repos/productRepo';
  import type { BranchProduct } from '$lib/models/BranchProduct';
  import type { Branch } from '$lib/models/Branch';
  import type { Product } from '$lib/models/Product';
  import Nav from '../../components/navbar/nav.svelte';
  import '../../styles/modal.css';
  import * as XLSX from 'xlsx';


  let branchProducts: BranchProduct[] = [];
  let branches: Branch[] = [];
  let products: Product[] = [];
  let selectedBranchName: string = '';
  let selectedProductName: string = '';
  let showModal = false;
  let isEditing = false;
  let branchProductInModal: Partial<BranchProduct> = {};

  onMount(async () => {
    try {
      branchProducts = await fetchBranchProducts();
      branches = await fetchBranches(); 
      products = await fetchProducts();
    } catch (error) {
      console.error('Error fetching branchProducts:', error);
    }
  });

  function openAddModal() {
    branchProductInModal = {
      branchName: '',
      productName: ''
    };
    isEditing = false;
    showModal = true;
  }
  async function handleSubmit() {
    const selectedBranch = branches.find(b => b.name === selectedBranchName);
    const selectedProduct = products.find(p => p.name === selectedProductName);

    if (selectedBranch && selectedProduct) {
      branchProductInModal = {
        branchName: selectedBranch.name,
        productName: selectedProduct.name
      };
      await assignProductToBranch(selectedBranch.name, selectedProduct.name);
      closeModal();
      window.location.reload();
    }
  }

  function closeModal() {
    showModal = false;
    selectedBranchName = '';
    selectedProductName = '';
  }

  async function handleDelete(id: number) {
    try {
      await deleteBranchProduct(id);
      branchProducts = branchProducts.filter(bp => bp.id !== bp.id);
      window.location.reload();
    } catch (error) {
      console.error('Error deleting branch product:', error);
    }
  }

  function downloadToExcel() {
    const ws = XLSX.utils.json_to_sheet(branchProducts.map(bp => ({
      'Branch Name': bp.branchName,
      'Product Name': bp.productName
    })));
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'BranchProducts');
    XLSX.writeFile(wb, 'BranchProducts.xlsx');
  }

</script>

<Nav/>

<div class="heading">
  <h1 class="page-title">Store Manager</h1>
  <button on:click={openAddModal} class="add">Assign Product to Branch</button>
  <button on:click={downloadToExcel} class="download">Download to Excel</button>
</div>

<ul>
  {#each branchProducts as branchProduct}
		<li>
			<div class="branch-product-container">
        <span class="branch-product-info">{branchProduct.branchName} - {branchProduct.productName}</span>
        <button on:click={() => handleDelete(branchProduct.id)} class='delete'>Delete</button>
			</div>
		</li>
	{/each}
</ul>

{#if showModal}
<div class="modal-overlay">
  <div class="modal">
    <div class="modal-title">Assign Product to Branch</div>
    <div class="form-div">
      <div class="form-input">
        <label for="product">Product:</label>
        <select id="product" bind:value={selectedProductName} class="dropdown">
          {#each products as product}
            <option value={product.name}>{product.name}</option>
          {/each}
        </select>
      </div>
      <div class="form-input">
        <label for="branch">Branch:</label>
        <select id="branch" bind:value={selectedBranchName} class="dropdown">
          {#each branches as branch}
            <option value={branch.name}>{branch.name}</option>
          {/each}
        </select>
      </div>
    </div>
    <div class="actions">
      <button type="button" on:click={handleSubmit}>Save</button>
      <button type="button" on:click={closeModal}>Cancel</button>
    </div>
  </div>
</div>
{/if}


<style>

.page-title{
    font-family: "Albert Sans", sans-serif;
		font-weight: 700;
		text-transform: uppercase;
		line-height: 1.5;
    font-size:60px;
    color: #00723F;
  }

  .heading{
    display: flex;
    align-items: center;
    justify-content: space-around;
  }

  ul {
		list-style-type: none;
		padding: 0;
		margin: 0; 
	}

	li {
		margin-bottom: 10px;
		padding: 15px;
		background-color: #f8f9fa; 
		border: 1px solid #dee2e6; 
		border-radius: 5px;
		font-family: "Albert Sans", sans-serif;
		font-weight: 700;
		text-transform: uppercase;
		line-height: 1.5;
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	li:hover {
		background-color: #e9ecef; 
	}

  button{
    margin-left: 5px;
    padding: 10px;
    font-family: "Albert Sans", Sans-serif;
    font-weight: 700;
    text-transform: uppercase;
    line-height: 1.5em;
    cursor: pointer;
    border-radius: 15px;
    border: #00723F;
  }

  .add {
		background-color: #00723F;
		color: white;
    margin-bottom: 1rem;
	}

  .delete {
		background-color: #dc3545;
		color: white;
	}

  .branch-product-container {
		display: flex;
		align-items: center;
		flex: 1; 
	}

	.branch-product-info {
		cursor: pointer;
		flex: 1; 
	}
</style>
