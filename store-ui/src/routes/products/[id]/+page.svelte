<script lang="ts">
  import { onMount } from 'svelte';
  import { fetchProduct } from '$lib/repos/productRepo';
  import type { Product } from '$lib/models/Product';
  import { goto } from '$app/navigation';
  import { page } from '$app/stores';
  import Nav from '../../../components/navbar/nav.svelte';

  const id = $page.params.id;
  let product: Product | null = null;

  onMount(async () => {
    try {
      product = await fetchProduct(id);
    } catch (error) {
      console.error('Error fetching product:', error);
    }
  });

  function handleBackToList() {
    goto('/products'); 
  }
</script>

<Nav/>

<h1>Product View</h1>
{#if product}
  <div class="product-container">
    <h2>{product.name}</h2>
    {#if product.weightedItem !== null}
      <p class="product-detail"><strong>Weighted Item:</strong> {product.weightedItem ? 'Yes' : 'No'}</p>
    {/if}

    {#if product.suggestedSellingPrice !== null}
      <p class="product-detail"><strong>Suggested Selling Price:</strong> R{product.suggestedSellingPrice.toFixed(2)}</p>
    {/if}
    <button on:click={handleBackToList} class="back-button">Back to Products List</button>
  </div>
{:else}
  <p>Loading product details...</p>
{/if}

<style>
  h1 {
    font-family: "Albert Sans", sans-serif;
    font-weight: 700;
    text-transform: uppercase;
    color: #00723F;
    font-size: 36px;
    text-align: center;
    margin-bottom: 20px;
  }

  .product-container {
    margin: 0 auto;
    padding: 20px;
    max-width: 600px;
    background-color: #f8f9fa;
    border: 1px solid #dee2e6;
    border-radius: 5px;
    text-align: center;
  }

  h2 {
    font-family: "Albert Sans", sans-serif;
    font-weight: 700;
    font-size: 28px;
    color: #333;
    margin-bottom: 10px;
  }

  .product-detail {
    font-family: "Albert Sans", sans-serif;
    font-weight: 400;
    font-size: 18px;
    color: #555;
    margin: 5px 0;
  }

  .back-button {
    margin-top: 20px;
    padding: 10px 20px;
    font-family: "Albert Sans", sans-serif;
    font-weight: 700;
    text-transform: uppercase;
    color: white;
    background-color: #00723F;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }

  .back-button:hover {
    background-color: #005c2d;
  }
</style>
