<script lang="ts">
  import { onMount } from 'svelte';
  import { fetchBranch } from '$lib/repos/branchRepo';
  import type { Branch } from '$lib/models/Branch';
  import { goto } from '$app/navigation';
  import { page } from '$app/stores';
  import Nav from '../../../components/navbar/nav.svelte';

  const id = $page.params.id;
  let branch: Branch | null = null;

  onMount(async () => {
    try {
      branch = await fetchBranch(id);
    } catch (error) {
      console.error('Error fetching branch:', error);
    }
  });

  function handleBackToList() {
    goto('/branches'); 
  }
</script>

<Nav/>

<h1>Branch View</h1>
{#if branch}
  <div class="branch-container">
    <h2>{branch.name}</h2>
      {#if branch.telephoneNumber !== null}
        <p class="branch-detail"><strong>Telephone Number:</strong> {branch.telephoneNumber}</p>
      {/if}

      {#if branch.openDate !== null}
        <p class="branch-detail"><strong>Open Date:</strong> {branch.openDate}</p>
      {/if}
    <button on:click={handleBackToList} class="back-button">Back to Branch List</button>
  </div>
{:else}
  <p>Loading branch details...</p>
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

  .branch-container {
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

  .branch-detail {
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
