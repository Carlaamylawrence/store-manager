<script lang="ts">
  import { onMount } from 'svelte';
  import { fetchBranches, addBranch, updateBranch, deleteBranch } from '$lib/repos/branchRepo';
  import type { Branch } from '$lib/models/Branch';
  import Nav from '../../components/navbar/nav.svelte';
  import { goto } from '$app/navigation';
  import '../../styles/modal.css';
  import * as XLSX from 'xlsx';

  let branches: Branch[] = [];
  let showModal = false;
  let isEditing = false;
  let branchInModal: Partial<Branch> = {};
  let error: string = '';

  onMount(async () => {
    try {
      branches = await fetchBranches();
    } catch (error) {
      console.error('Error fetching branches:', error);
    }
  });

  function openAddModal() {
    branchInModal = {
      id:'',
      name: '',
      telephoneNumber: '',
      openDate: null
    };
    isEditing = false;
    showModal = true;
  }

  function openEditModal(branch: Branch) {
    branchInModal = {
      ...branch,
      openDate: branch.openDate ? new Date(branch.openDate).toISOString().split('T')[0] : null // Ensure the date is formatted to YYYY-MM-DD
    };
    isEditing = true;
    showModal = true;
  }

  async function handleSubmit() {
      const { id, name, telephoneNumber, openDate } = branchInModal;

      if (isEditing && id) {
        const updatedBranch = await updateBranch({
          id,
          name,
          telephoneNumber,
          openDate
        });

        if (updatedBranch) {
				branches = branches.map((p) => (p.id === id ? updatedBranch : p));
				closeModal();
				window.location.reload();
        }
      } else {
        const addedBranch = await addBranch({
          id,
          name,
          telephoneNumber,
          openDate
        });
        if (addedBranch) {
        branches = [...branches, addedBranch];
        closeModal();
			}
    }
      closeModal();
  }

  async function handleDeleteBranch(id: number) {
    try {
      await deleteBranch(id);
      branches = branches.filter(branch => branch.id !== id);
    } catch (error) {
      console.error('Error deleting branch:', error);
      showError('Error deleting branch  as it exists in the StoreManager. Please try again.');
    }
  }

  function closeModal() {
    showModal = false;
    branchInModal = {};
    fetchBranches();
  }

  function showError(message: string) {
		error = message;
		setTimeout(() => {
			error = '';
		}, 5000);
	}

  function navigateToBranchDetail(id: number) {
		goto(`/branches/${id}`);
	}

  function downloadToExcel() {
    const ws = XLSX.utils.json_to_sheet(branches.map(b => ({
			'id': b.id,
      'Branch Name': b.name,
      'OpenDate': b.openDate,
			'TelephoneNumber': b.telephoneNumber
    })));
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Branches');
    XLSX.writeFile(wb, 'Branches.xlsx');
  }
</script>
<Nav/>

<div class="heading">
  <h1 class="page-title">Branches</h1>
  <button on:click={openAddModal} class="add">Add New Branch</button>
  <button on:click={downloadToExcel} class="download">Download to Excel</button>
</div>

{#if error}
	<div class="error-message">{error}</div>
{/if}

<ul>
  {#each branches as branch}
		<li>
			<div class="branch-container">
        <span class="branch-id">{branch.id}</span>
				<span class="branch-name" on:click={() => navigateToBranchDetail(branch.id)}>{branch.name}</span>
				<div class="button-container">
					<button on:click={() => openEditModal(branch)} class='edit'>Edit</button>
					<button on:click={() => handleDeleteBranch(branch.id)} class='delete'>Delete</button>
				</div>
			</div>
		</li>
	{/each}
</ul>

{#if showModal}
  <div class="modal-overlay">
    <div class="modal">
      <div class="modal-title">
        <h2>{isEditing ? "Edit Branch" : "Add Branch"}</h2>
      </div>
      <div class="form-div">
        <form on:submit|preventDefault={handleSubmit}>
          <div class="form-input">
            <label for="id">ID</label>
            <input id="id" type="number" bind:value={branchInModal.id} />
          </div>
          <div class="form-input">
            <label for="name"> Branch Name</label>
            <input id="name" type="text" bind:value={branchInModal.name} required />
          </div>
          <div class="form-input">
            <label for="telephoneNumber">Telephone Number</label>
            <input id="telephoneNumber" type="text" bind:value={branchInModal.telephoneNumber} />
          </div>
          <div class="form-input">
            <label for="openDate">Open Date</label>
            <input id="openDate" type="date" bind:value={branchInModal.openDate} />
          </div>
          <div class="actions">
            <button type="submit">{isEditing ? "Save Changes" : "Add Branch"}</button>
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

	.branch-container {
		display: flex;
		align-items: center;
		flex: 1; 
	}

	.branch-name {
		cursor: pointer;
		flex: 1; 
	}

  .branch-id{
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
