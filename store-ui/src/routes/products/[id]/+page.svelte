<script lang="ts">
  import { onMount } from 'svelte';
  import { fetchProduct } from '$lib/repos/productRepo';
  import type { Product } from '$lib/models/Product';
  import { goto } from '$app/navigation';
  import {page} from '$app/stores';

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
<h1> Single {id} Product View</h1>
{#if product}
  <div>
    <h1>{product.name}</h1>
    <p>Weighted Item: {product.weightedItem ? 'Yes' : 'No'}</p>
    <p>Suggested Selling Price: R{product.suggestedSellingPrice.toFixed(2)}</p>
    <button on:click={handleBackToList}>Back to Products List</button>
  </div>
{:else}
  <p>Loading product details...</p>
{/if}
