<script lang="ts">
	import { onMount } from 'svelte';
	import { fetchProducts, deleteProduct, addProduct, updateProduct, uploadProducts } from '$lib/repos/productRepo';
	import type { Product } from '$lib/models/Product';
	import Nav from '../../components/navbar/nav.svelte';
	import { goto } from '$app/navigation';
	import '../../styles/modal.css';
	import * as XLSX from 'xlsx';

	let products: Product[] = [];
	let showModal = false;
	let isEditing = false;
	let productInModal: Partial<Product> = {};
	let error: string = '';
	let selectedFile: File | null = null;


	onMount(async () => {
		try {
			products = await fetchProducts();
		} catch (error) {
			console.error('Error fetching products:', error);
		}
	});

	function openAddModal() {
		productInModal = {
			id: '',
			name: '',
			weightedItem: false,
			suggestedSellingPrice: 0
		};
		isEditing = false;
		showModal = true;
	}

	function openEditModal(product: Product) {
		productInModal = { ...product };
		isEditing = true;
		showModal = true;
	}

	async function handleSubmit() {
		const { id, name, weightedItem, suggestedSellingPrice } = productInModal;

		if (isEditing && id) {
			const updatedProduct = await updateProduct({
            id,
            name,
            weightedItem,
            suggestedSellingPrice
        });

			if (updatedProduct) {
				products = products.map((p) => (p.id === id ? updatedProduct : p));
				closeModal();
				window.location.reload();
			}
		} else {
        try {
            const addedProduct = await addProduct({
                id,
                name,
                weightedItem,
                suggestedSellingPrice
            });
            if (addedProduct) {
                products = [...products, addedProduct];
                closeModal();
            }
        } catch (error) {
            console.error('Failed to add product:', error);
            showError('Failed to add product. Please try again.');
        }
    }
    closeModal();
}
	
	async function handleDeleteProduct(id: number) {
		try {
			await deleteProduct(id);
			products = products.filter((p) => p.id !== id);
			window.location.reload();
		} catch (error) {
			console.error('Error deleting product:', error);
			showError('Error deleting product as it exists in the StoreManager. Please try again.');
		}
	}

	function closeModal() {
		showModal = false;
		productInModal = {};
	}

	function showError(message: string) {
		error = message;
		setTimeout(() => {
			error = '';
		}, 5000);
	}

	function navigateToProductDetail(id: number) {
		goto(`/products/${id}`);
	}

	function downloadToExcel() {
    const ws = XLSX.utils.json_to_sheet(products.map(p => ({
			'id': p.id,
      'Product Name': p.name,
      'SuggestedSellingPrice': p.suggestedSellingPrice,
			'WeightedItem': p.weightedItem
    })));
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Products');
    XLSX.writeFile(wb, 'Products.xlsx');
  }

	function handleFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      selectedFile = input.files[0];
    } else {
      selectedFile = null;
    }
  }

  async function handleFileUpload() {
    if (!selectedFile) {
      showError('Please select a file to upload.');
      return;
    }

    try {
      const formData = new FormData();
      formData.append('file', selectedFile);

      await uploadProducts(formData);
      products = await fetchProducts();
      selectedFile = null;
    } catch (error) {
      console.error('Failed to upload file:', error);
      showError('Failed to upload file. Please try again.');
    }
  }
</script>

<Nav />
<div class="heading">
  <h1 class="page-title">Products</h1>
  <button on:click={openAddModal} class="add">Add New Product</button>
	<button on:click={downloadToExcel} class="download">Download to Excel</button>
	<div class="import">
		<input type="file" class="import-input" on:change={handleFileChange} />
		<button on:click={handleFileUpload}>Upload File</button>
  </div>
</div>

{#if error}
	<div class="error-message">{error}</div>
{/if}

<ul>
	{#each products as product}
		<li>
			<div class="product-container">
        <span class="product-id">{product.id}</span>
				<span class="product-name" on:click={() => navigateToProductDetail(product.id)}>{product.name}</span>
				<div class="button-container">
					<button on:click={() => openEditModal(product)} class='edit'>Edit</button>
					<button on:click={() => handleDeleteProduct(product.id)} class='delete'>Delete</button>
				</div>
			</div>
		</li>
	{/each}
</ul>

{#if showModal}
	<div class="modal-overlay">
		<div class="modal">
			<div class="modal-title">
				<h2>{isEditing ? 'Edit Product' : 'Add Product'}</h2>
			</div>
			<div class="form-div">
				<form on:submit|preventDefault={handleSubmit}>
					<div class="form-input">
						<label for="id">ID</label>
						<input id="id" type="number" bind:value={productInModal.id} disabled={isEditing}/>
					</div>
					<div class="form-input">
						<label for="name">Name</label>
						<input id="name" type="text" bind:value={productInModal.name} required />
					</div>
					<div class="form-input">
						<label for="weightedItem">Weighted Item</label>
						<input id="weightedItem" type="checkbox" bind:checked={productInModal.weightedItem} />
					</div>
					<div class="form-input">
						<label for="suggestSellingPrice">Suggest Selling Price (R)</label>
						<input
							id="suggestSellingPrice"
							type="number"
							step="0.01"
							bind:value={productInModal.suggestedSellingPrice}
							required
						/>
					</div>
					<div class="actions">
						<button type="submit">{isEditing ? 'Save Changes' : 'Add Product'}</button>
						<button type="button" on:click={closeModal}>Cancel</button>
					</div>
				</form>
			</div>
		</div>
	</div>
{/if}

<style>
  .heading{
    display: flex;
    align-items: center;
    justify-content: space-around;
  }

	.import{
		background-color: #478a6c2f;
		border-radius: 15px;
	}

	.import-input{
		margin-left: 5px;
    padding: 10px;
    font-family: "Albert Sans", Sans-serif;
    font-weight: 700;
    text-transform: uppercase;
    line-height: 1.5em;
	}

  .page-title{
    font-family: "Albert Sans", sans-serif;
		font-weight: 700;
		text-transform: uppercase;
		line-height: 1.5;
    font-size:60px;
    color: #00723F;
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

	.product-container {
		display: flex;
		align-items: center;
		flex: 1; 
	}

	.product-name {
		cursor: pointer;
		flex: 1; 
	}

  .product-id{
    padding-right: 1rem;
  }

	.button-container {
		display: flex;
		align-items: center;
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

	.edit {
		background-color: #ffc107;
		color: black;
	}

	.delete {
		background-color: #dc3545;
		color: white;
	}

	.error-message {
		background-color: #f8d7da;
		color: #721c24;
		padding: 10px;
		border: 1px solid #f5c6cb;
		border-radius: 5px;
		margin-bottom: 10px;
	}
</style>
