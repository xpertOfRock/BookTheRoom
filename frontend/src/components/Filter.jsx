import { Select, Input } from "@chakra-ui/react";

function Filter({filter, setFilter}) {
    return (
        <div className="flex flex-col gap-5">
            <label>Search:</label>
            <Input placeholder="..." onChange={(e) => setFilter({...filter, search: e.target.value})}/>
            <label>Sort by:</label>
            <Select onChange={(e) => setFilter({...filter, sortItem: e.target.value})}>
                <option>---</option>
                <option value={"name"}>Name</option>
                <option value={"rating"}>Rating</option>
            </Select>
            <label>Order by:</label>
            <Select onChange={(e) => setFilter({...filter, sortOrder: e.target.value})}>
                <option>---</option>
                <option value={"asc"}>Ascending</option>
                <option value={"desc"}>Descending</option>
            </Select>
        </div>
    )
}

export default Filter;